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
    	["GuardTest"] = "res://screens/world/testing_levels/guard_test_scene.tscn",
    	["AllyTest"] = "res://screens/world/testing_levels/ally_test_scene.tscn.tscn",
		["Prison"] = "res://screens/world/testing_levels/ally_test_scene.tscn.tscn",
	};

	public string NextScene = "";
	public string currentSceneName = "GuardTest";
	public async Task TransitionScene(string sceneName)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		currentSceneName = sceneName;
		GetTree().ChangeSceneToFile(sceneDict[sceneName]);
	}

	public async Task TransitionWorldScene(string sceneName,int playerInstantiatorId){
		
		// Example: Set specific zoom levels per scene
		if (sceneName == "Field") 
			CurrentZoom = new Vector2(0.8f, 0.8f); // Zoom out a bit
		else
			CurrentZoom = new Vector2(1.0f, 1.0f); // Standard zoom

		await TransitionScene(sceneName);
		await ToSignal(GetTree(), SceneTree.SignalName.SceneChanged);
		
		// The camera's _Ready() will now pull the updated CurrentZoom
		DoPlayerInstantiation(playerInstantiatorId);

		// await TransitionScene(sceneName);
		// await ToSignal(GetTree(), SceneTree.SignalName.SceneChanged);
		// PrintAllNodes();
		// DoPlayerInstantiation(playerInstantiatorId);
		PrintAllNodes();
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
		DoPlayerInstantiation(0);
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

	public void SaveGame()
	{
		var data = new SaveData
		{
			CurrentSceneName = currentSceneName,
			ZoomX = CurrentZoom.X,
			ZoomY = CurrentZoom.Y,
			PlayerPosX = player.GlobalPosition.X,
			PlayerPosY = player.GlobalPosition.Y
		};

		foreach (var pair in allyDict)
		{
			data.Allies.Add(new AllySaveEntry
			{
				Id = pair.Key,
				IsFollowing = pair.Value["isFollowing"].AsBool(),
				IsImprisoned = pair.Value["isImprisoned"].AsBool(),
				PosX = pair.Value["position"].AsVector2().X,
				PosY = pair.Value["position"].AsVector2().Y,
				SceneName = pair.Value["sceneName"].AsString()
			});
		}

		string jsonString = JsonSerializer.Serialize(data);
		using var file = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Write);
		file.StoreString(jsonString);
		GD.Print("Game Saved!");
	}

	public async void LoadGame()
	{
		if (!FileAccess.FileExists("user://savegame.json")) return;

		using var file = FileAccess.Open("user://savegame.json", FileAccess.ModeFlags.Read);
		string jsonString = file.GetAsText();
		SaveData data = JsonSerializer.Deserialize<SaveData>(jsonString);

		// 1. Restore Global State
		currentSceneName = data.CurrentSceneName;
		CurrentZoom = new Vector2(data.ZoomX, data.ZoomY);

		foreach (var ally in data.Allies)
		{
			if (allyDict.ContainsKey(ally.Id))
			{
				allyDict[ally.Id]["isFollowing"] = ally.IsFollowing;
				allyDict[ally.Id]["isImprisoned"] = ally.IsImprisoned;
				allyDict[ally.Id]["position"] = new Vector2(ally.PosX, ally.PosY);
				allyDict[ally.Id]["sceneName"] = ally.SceneName;
			}
		}

		// 2. Transition to scene
		// We use a flag or temporary variable if we want the player to snap to their saved pos
		// instead of the Instantiator's default pos.
		await TransitionWorldScene(currentSceneName, 0); 
		
		// 3. Optional: Manually set player position if you don't want them at the spawn point
		player.GlobalPosition = new Vector2(data.PlayerPosX, data.PlayerPosY);
		
		GD.Print("Game Loaded!");
	}
}