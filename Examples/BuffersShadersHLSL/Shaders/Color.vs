cbuffer MatrixBuffer {
	matrix World;
	matrix View;
	matrix Projection;
};

struct VertexInputType {
	float4 position : POSITION;
	float4 color : COLOR;
};

struct PixelInputType {
	float4 position : SV_POSITION;
	float4 color : COLOR;
};

PixelInputType ColorVertexShader(VertexInputType input) {
	PixelInputType output;

	input.position.w = 1.0f;

	output.position = mul(input.position, World);
	output.position = mul(output.position, View);
	output.position = mul(output.position, Projection);
	output.color = input.color;

	return output;
}