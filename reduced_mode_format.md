# Saving to File

### Reduced Mode File Format
```
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
R_(0,0) R_(1,0) ... R_(n-1,0)
...     ...     ... ...
R_(0,m-1) R_(1,m-1) ... R_(n-1,m-1)
END
```

#### Notes:
* damping normalized between 0 and 1
* only uses audible frequencies (relevant if scaling frequencies)
* Each column of R is the gain values of the vertex (column index)
* All gain values normalized between 0 and 1