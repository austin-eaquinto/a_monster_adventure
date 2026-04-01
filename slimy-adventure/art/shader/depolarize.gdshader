shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
uniform vec2 depolarize_pixel_dir = vec2(5,5);

void fragment() {
	// Called for every pixel the material is visible on.
	
	vec2 screen_pixel_pos = SCREEN_UV / SCREEN_PIXEL_SIZE;
	
	vec2 red_pixel_pos = screen_pixel_pos + depolarize_pixel_dir;
	vec2 green_pixel_pos = screen_pixel_pos;
	vec2 blue_pixel_pos = screen_pixel_pos - depolarize_pixel_dir;
	
	vec2 red_screen_uv = red_pixel_pos * SCREEN_PIXEL_SIZE;
	vec2 green_screen_uv = green_pixel_pos * SCREEN_PIXEL_SIZE;
	vec2 blue_screen_uv = blue_pixel_pos * SCREEN_PIXEL_SIZE;
	
	vec4 bg_red = textureLod(screen_texture, red_screen_uv, 0.0);
	vec4 bg_green = textureLod(screen_texture, green_screen_uv, 0.0);
	vec4 bg_blue = textureLod(screen_texture, blue_screen_uv, 0.0);
	
	COLOR = vec4(bg_red.r,bg_green.g,bg_blue.b,bg_green.a);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
