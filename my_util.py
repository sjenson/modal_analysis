import numpy as np

def load_obj(file_name):
    V = []
    F = []

    with open(file_name, 'r') as f:
        for line in f:
            if len(line) == 0 or line[0] == '#':
                continue

            # vertex
            if line[0] == 'v':
                line = np.array([float(x) for x in line.strip().split(' ')[1:]])
                V.append(line)

            # face
            if line[0] == 'f':
                line = [int(x) - 1 for x in line.strip().split(' ')[1:]]
                F.append(line)
                
    edge_set = set()
    for face in F:
        for i in range(3):
            if (face[i] < face[(i+1)%3]):
                edge_set.add((face[i], face[(i+1)%3]))
            else:
                edge_set.add((face[(i+1)%3], face[i]))
                
    return V, F, edge_set


def unit_vector(vector):
    """ Returns the unit vector of the vector.  """
    return vector / np.linalg.norm(vector)

def angle_between(v1, v2):
    """ Returns the angle in radians between vectors 'v1' and 'v2'::

            >>> angle_between((1, 0, 0), (0, 1, 0))
            1.5707963267948966
            >>> angle_between((1, 0, 0), (1, 0, 0))
            0.0
            >>> angle_between((1, 0, 0), (-1, 0, 0))
            3.141592653589793
    """
    v1_u = unit_vector(v1)
    v2_u = unit_vector(v2)
    return np.arccos(np.clip(np.dot(v1_u, v2_u), -1.0, 1.0))