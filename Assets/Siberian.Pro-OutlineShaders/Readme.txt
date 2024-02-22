This plugin works in VR and Standalone mode 
===

Implemented 3 types of plug-in with two variations.
1. Object Outline

Implemented in two passes - creates an enlarged copy of the object and renders it in the background. 
Suitable for simple geometry and mobile devices. However, in the case of complex geometry or holes in the mesh, 
it is not possible to outline the inner contour. As a variation, we added a more complex object outline with  
addition of blur to "double" (see the second part of the video).

2. Vertex Outline

It is implemented by displacement of vertices in the direction of the normal from the vertex. 
This way more accurately describes various complex meshes, but is limited by change of thickness. 
As a variation, implemented two shaders: affected with light and unlit.

3. Edge Outline

Each edge is placed in correspondence with a parallel edge, displaced towards the normal. 
To simplify the calculations of  intersection point was added "smooth" property. This is most demanding to 
the performance approach, but the most accurate for the outline of proposed ones.

+ Full source code of the shader is included - you can modify and combine variants of non-multiple approaches.
+ The properties for the color, outline width, textures are carried out. Depending on the shader, additional properties are rendered, such as the visualization of the object itself (you can leave only the outline), the degree of blurring, rotation or bias of the double. You can easily animate them.
+ Ready-to-go asset. Easily deployable in existing project.
+ Developed with love <3

---

Support: kv@siberian.pro



