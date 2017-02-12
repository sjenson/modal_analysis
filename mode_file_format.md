# Saving to File

### Mode File Format
`
number of vertices (n)
number of triangles (t)
number of modes (m)
vertex_0.x vertex_0.y vertex_0.z vertex_0.weight 
vertex_1.x vertex_1.y vertex_1.z vertex_1.weight
...  
vertex_(n-1).x vertex_(n-1).y vertex_(n-1).z vertex_(n-1).weight
triangle_0.index_0 triangle_0.index_1 triangle_0.index_2  
...  
triangle_(n-1).index_0 triangle_(n-1).index_1 triangle_(n-1).index_2
freq_0 (in Hz)
freq_1 
...
freq_(m-1)
damping_0
damping_1
...
damping(m-1)
G_(0,0) G_(1,0) ... G_((3*n)-1,0)
...     ...     ... ...
G_(0,m) G_(1,m) ... G_((3*n)-1,m)
END
`

#### Notes:
* damping normalized between 0 and 1
* only uses audible frequencies (relevant if scaling frequencies)
* G is actually G<sup>-1</sup>, and shaped to correspond to audible modes