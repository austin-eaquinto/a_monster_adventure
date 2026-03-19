using Godot;
using System;
using Godot.Collections;
using System.Threading.Tasks;
using System.Text.Json; // for data saves

public partial class Global : Node
{
	
	public static Global Instance { get; private set; }
	public Player player {get; set;}
	public Vector2 CurrentZoom { get; set; } = new Vector2(1.0f, 1.0f);
	public bool IsLoadingFromSave { get; private set; } = false;
    private TaskCompletionSource<bool> _transitionTask;

	[Signal]
	public delegate void AlertGuardsEventHandler(Vector2 alertPosition, Character spottedPrisoner);

	public Dictionary<int,Dictionary<string,Variant>> allyDict = new Dictionary<int,Dictionary<string,Variant>>
        {
            { 0, new Dictionary<string, Variant>
				{
					{"isFollowing", false},
					{"isImprisoned", true},
					{"sceneName", ""},
					{"position", new Vector2(0,0)},
					{"allyScene", "res://characters/allies/ghost_ally.tscn"}
				} 
			},
            { 1, new Dictionary<string, Variant>
				{
					{"isFollowing", false},
					{"isImprisoned", true},
					{"sceneName", ""},
					{"position", new Vector2(0,0)},
					{"allyScene", "res://characters/allies/ally.tscn"}
				} 
			},
            { 2, new Dictionary<string, Variant>
				{
					{"isFollowing", false},
					{"isImprisoned", true},
					{"sceneName", ""},
					{"position", new Vector2(0,0)},
					{"allyScene", "res://characters/allies/ally.tscn"}
				} 
			}
        };
	
	public Dictionary<string,string> sceneDict = new Dictionary<string, string>
	{
		["NA"] = "",
		["LoadingScreen"] = "res://screens/loading/loading_screen.tscn",
		["MainMenu"] = "res://screens/main_menu/main_menu.tscn", // Added this
		["GuardTest"] = "res://screens/world/testing_levels/guard_test_scene.tscn",
		["AllyTest"] = "res://screens/world/testing_levels/ally_test_scene.tscn", // Fixed .tscn.tscn
		["Prison"] = "res://screens/world/prison/prison.tscn", // Fixed path and double extension
		["Field"] = "res://screens/world/field/field.tscn",
	};

	public string NextScene = "";
	public string currentSceneName = "GuardTest";
	// Change 'void' to 'Task' so it can be awaited
	public async Task TransitionScene(string sceneName)
    {
        if (!sceneDict.ContainsKey(sceneName))
        {
            GD.PrintErr($"Transition Error: {sceneName} not found!");
            return;
        }

        GetTree().Paused = false;
        NextScene = sceneDict[sceneName];
        currentSceneName = sceneName;

        if (sceneName == "MainMenu")
        {
            var camera = GetTree().GetFirstNodeInGroup("GameCamera") as Camera2D;
            if (camera != null) camera.GlobalPosition = Vector2.Zero;
        }

        // Initialize the tracker so we can await the end of the transition
        _transitionTask = new TaskCompletionSource<bool>();
        
        GetTree().ChangeSceneToFile(sceneDict["LoadingScreen"]);

        // Wait here until the LoadingScreen or OnNodeEnteredTree signals completion
        await _transitionTask.Task;
        GD.Print($"Transition to {sceneName} complete.");
    }

	public async Task TransitionWorldScene(string sceneName, int playerInstantiatorId)
    {
        CurrentZoom = (sceneName == "Field") ? new Vector2(0.8f, 0.8f) : new Vector2(1.0f, 1.0f);
        await TransitionScene(sceneName);
    }

	public void DoPlayerInstantiation(int playerInstantiatorId)
	{
		Array<PlayerInstantiator> playerInstantiators = GetPlayerInstantiators(GetTree().Root);

		foreach (PlayerInstantiator playerInstantiator in playerInstantiators)
		{
			if (playerInstantiator.id == playerInstantiatorId)
			{
				GD.Print(playerInstantiatorId);
				playerInstantiator.DoInstantiation();
				break;
			}
		}
	}

	public Array<PlayerInstantiator> GetPlayerInstantiators(Node currentNode)
	{
		Array<PlayerInstantiator> playerInstantiators = [];

		foreach (Node child in currentNode.GetChildren())
		{
			if (child is PlayerInstantiator) playerInstantiators.Add(child as PlayerInstantiator);
			else playerInstantiators += GetPlayerInstantiators(child);
		}

		return playerInstantiators;
	}
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		
		// Connect to the scene_changed signal so we know when the LoadingScreen finishes
		GetTree().Root.ChildEnteredTree += OnNodeEnteredTree;
	}

	private void OnNodeEnteredTree(Node node)
	{
		// Convert to lowercase to avoid "LoadingScreen" vs "loading_screen" mismatches
		string nodeName = node.Name.ToString().ToLower();

		// 1. Identify if we are in a playable world scene
		bool isWorld = nodeName != "mainmenu" && 
					nodeName != "loading_screen" && 
					nodeName != "root" && 
					nodeName != "pausemenu";

		if (isWorld)
		{
			// Only instantiate player/allies if it's a world scene
			CallDeferred(nameof(DoPlayerInstantiation), 0);
		}

		// 2. Resolve the transition task regardless of scene type
		// This unblocks the 'await' in TransitionScene and LoadGame
		if (_transitionTask != null && !_transitionTask.Task.IsCompleted)
		{
			_transitionTask.TrySetResult(true);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PrintAllNodes(Node currentNode = null)
	{
		if (currentNode == null)
		{
			PrintAllNodes(GetTree().Root);
			return;
		}

		foreach (Node child in currentNode.GetChildren())
		{
			GD.Print(child," : ",child.Name);
			PrintAllNodes(child);
		}
	}

	public bool IsSaving { get; private set; } = false; // Add this variable at the top of your class

	public void SaveGame()
	{
		// 0. LOCK THE STATE
		if (IsSaving) return; 
		IsSaving = true;

		try 
		{
			// 1. TRY TO RECOVER THE PLAYER REFERENCE FIRST
			if (player == null || !IsInstanceValid(player))
			{
				player = GetTree().GetFirstNodeInGroup("Player") as Player;
			}

			// 2. NOW CHECK IF WE ACTUALLY HAVE A PLAYER
			if (player == null)
			{
				GD.PrintErr("Save failed: No Player found in tree or Global reference!");
				return;
			}

			// 3. INITIALIZE THE DATA OBJECT ONCE
			var data = new SaveData
			{
				CurrentSceneName = currentSceneName,
				ZoomX = CurrentZoom.X,
				ZoomY = CurrentZoom.Y,
				PlayerPosX = player.GlobalPosition.X,
				PlayerPosY = player.GlobalPosition.Y
			};

			// 4. ADD ALLIES TO THE SAME 'data' OBJECT
			foreach (var pair in allyDict)
			{
				if (pair.Value == null) continue;

				data.Allies.Add(new AllySaveEntry
				{
					Id = pair.Key,
					IsFollowing = pair.Value.ContainsKey("isFollowing") ? pair.Value["isFollowing"].AsBool() : false,
					IsImprisoned = pair.Value.ContainsKey("isImprisoned") ? pair.Value["isImprisoned"].AsBool() : true,
					PosX = pair.Value.ContainsKey("position") ? pair.Value["position"].AsVector2().X : 0,
					PosY = pair.Value.ContainsKey("position") ? pair.Value["position"].AsVector2().Y : 0,
					SceneName = pair.Value.ContainsKey("sceneName") ? pair.Value["sceneName"].AsString() : ""
				});
			}

			// 5. SERIALIZE AND SAVE
			string jsonString = JsonSerializer.Serialize(data);
			
			using (var file = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Write))
			{
				if (file == null)
				{
					GD.PrintErr("Failed to open file for writing at user://savegame.json");
					return;
				}
				file.StoreString(jsonString);
				file.Flush(); 
			} 
			
			GD.Print("Game Saved Successfully!");
		}
		catch (Exception e)
		{
			GD.PrintErr($"Failed to serialize save data: {e.Message}");
		}
		finally
		{
			// 6. UNLOCK THE STATE
			// This runs no matter what, even if an error occurred above
			IsSaving = false;
		}
		GD.Print($"Save Finished. Current Scene Key is: {currentSceneName}");
	}

	public async void LoadGame()
	{
		if (!FileAccess.FileExists("user://savegame.json"))
		{
			GD.Print("Load failed: No save file found.");
			return;
		}

		IsLoadingFromSave = true; 

		using var file = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Read);
		string jsonString = file.GetAsText();
		SaveData data = JsonSerializer.Deserialize<SaveData>(jsonString);

		// 1. Restore persistent Global State
		currentSceneName = data.CurrentSceneName;
		CurrentZoom = new Vector2(data.ZoomX, data.ZoomY);

		// 2. Transition to the saved scene and wait for completion
		// This awaits the _transitionTask we set up in TransitionScene
		await TransitionWorldScene(currentSceneName, 0);

		// 3. Wait for Deferred Calls
		// Wait two frames: one for the scene to settle, 
		// and one for the PlayerInstantiator's CallDeferred("add_child") to finish.
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		// 4. Restore Player Position
		player = GetTree().GetFirstNodeInGroup("Player") as Player;
		if (player != null)
		{
			player.GlobalPosition = new Vector2(data.PlayerPosX, data.PlayerPosY);
			
			var camera = GetTree().GetFirstNodeInGroup("GameCamera") as GameCamera;
			if (camera != null)
			{
				camera.SetTarget(player);
				camera.SnapToTarget();
			}
		}

		// 5. Restore Ally Positions and State
		foreach (var allyEntry in data.Allies)
		{
			// Search the current scene for the instantiated ally with a matching ID
			var allies = GetTree().GetNodesInGroup("Allies"); 
			foreach (Node node in allies)
			{
				if (node is Ally a && a.id == allyEntry.Id)
				{
					// Snap to saved position
					a.GlobalPosition = new Vector2(allyEntry.PosX, allyEntry.PosY);
					
					// Sync the Global dictionary back to the saved state
					if (allyDict.ContainsKey(a.id))
					{
						allyDict[a.id]["isFollowing"] = allyEntry.IsFollowing;
						allyDict[a.id]["isImprisoned"] = allyEntry.IsImprisoned;
						allyDict[a.id]["position"] = a.GlobalPosition;
						allyDict[a.id]["sceneName"] = allyEntry.SceneName;
					}
					break;
				}
			}
		}
		
		IsLoadingFromSave = false;
		GD.Print("Game Loaded and Player/Allies Positioned!");
	}
}