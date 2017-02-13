# Saving to File

### Mode File Format
`
number of vertices (n)<br />
number of triangles (t)<br />
number of modes (m)<br />
vertex_0.x vertex_0.y vertex_0.z vertex_0.weight<br />
vertex_1.x vertex_1.y vertex_1.z vertex_1.weight<br />
...  <br />
vertex_(n-1).x vertex_(n-1).y vertex_(n-1).z vertex_(n-1).weight<br />
triangle_0.index_0 triangle_0.index_1 triangle_0.index_2  <br />
...  <br />
triangle_(n-1).index_0 triangle_(n-1).index_1 triangle_(n-1).index_2<br />
freq_0 (in Hz)<br />
freq_1 <br />
...<br />
freq_(m-1)<br />
damping_0<br />
damping_1<br />
...<br />
damping(m-1)<br />
G_(0,0) G_(1,0) ... G_((3*n)-1,0)<br />
...     ...     ... ...<br />
G_(0,m) G_(1,m) ... G_((3*n)-1,m)<br />
END<br />
`

#### Notes:
* damping normalized between 0 and 1
* only uses audible frequencies (relevant if scaling frequencies)
* G is actually G<sup>-1</sup>, and shaped to correspond to audible modes
