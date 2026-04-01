shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;
uniform float pixel_size = 4.0f;

void fragment() {
	// Called for every pixel the material is visible on.
	
	vec2 screen_pixel_pos = SCREEN_UV / SCREEN_PIXEL_SIZE;
	vec2 pixelated_screen_pixel_pos = screen_pixel_pos + vec2(pixel_size/2.0f) - mod(screen_pixel_pos + vec2(pixel_size/2.0f),pixel_size);
	vec2 pixelated_screen_uv = pixelated_screen_pixel_pos * SCREEN_PIXEL_SIZE;
	
	vec4 bg = textureLod(screen_texture, pixelated_screen_uv, 0.0);
	
	COLOR = bg;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
