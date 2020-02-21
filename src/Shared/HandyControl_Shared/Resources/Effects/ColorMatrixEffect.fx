sampler2D implicitInput : register(s0);
float a : register(c0);
float b : register(c1);
float c : register(c2);
float d : register(c3);
float e : register(c4);

float f : register(c5);
float g : register(c6);
float h : register(c7);
float i : register(c8);
float j : register(c9);

float k : register(c10);
float l : register(c11);
float m : register(c12);
float n : register(c13);
float o : register(c14);

float p : register(c15);
float q : register(c16);
float r : register(c17);
float s : register(c18);
float t : register(c19);

/*

     --             --        --   --
     | a  b  c  d  e |        |  R  |
     | f  g  h  i  j |        |  G  |
 A = | k  l  m  n  o |   C =  |  B  |  R = AC
     | p  q  r  s  t |        |  A  |
     --             --        |  1  |
                              --   --

*/

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(implicitInput, uv);
    
    float4x4 matrixA1 =
    {
        a, b, c, d,
        f, g, h, i,
        k, l, m, n,
        p, q, r, s
    };
    
    return mul(matrixA1, color) + float4( e, j, o, t );
}