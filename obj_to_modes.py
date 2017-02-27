import numpy as np
import math
import sys
import scipy.io.wavfile
from my_util import load_obj, unit_vector, angle_between
from scipy.linalg import eig, norm
import matplotlib.pyplot as plt
from pickle import dump, load


if len(sys.argv) != 2:
    print("Usage: python obj_to_modes.py input_name \n\
        input_name should be obj file in meshes folder.")
    exit()
else:
    mesh_file = "meshes/" + sys.argv[1]


print("Loading meshes and constructing matrices.")
# Load mesh and construct matrices

# Material properties
# Glass from example-based modal synthesis paper (Lin)
Y = 6.2e10
density = 2600
thickness = 0.1

alpha = η = 1.8301e2
beta = γ = 1.4342e-7

k = Y*thickness

# Load file using my_util loader
V, faces, edges = load_obj(mesh_file)

# Extract other relevant information from the mesh including edge length and weight

edge_lengths = np.zeros((len(edges),))
for i, e in enumerate(edges):
    edge_lengths[i] = norm((V[e[0]] - V[e[1]]))

vertex_weights = np.zeros((len(V),))
for face in faces:
    # get area of the face
    a = V[face[0]]
    b = V[face[1]]
    c = V[face[2]]
    
    theta = angle_between(b-a, c-a)
    area = .5*norm(b-a)*norm(c-a)*math.sin(theta)
    
    for v in face:
        vertex_weights[v] += area
        

# Calculate the stiffness matrix
# (K_ij = k / l * v * v^T)

dim = len(V)*3
K = np.zeros((dim, dim))

for i, edge in enumerate(edges):
    # compute the direction unit vector
    unit = V[edge[1]] - V[edge[0]]
    unit /= norm(unit)
    unit.shape = (3, 1)
    
    submatrix = (k/edge_lengths[i])*unit.dot(unit.T)
    
    x = edge[0]*3
    y = edge[1]*3
    K[x:x+3, y:y+3] = submatrix
    K[y:y+3, x:x+3] = submatrix.T
    
print("Calculating eigendecomposition.")
# Eigendecomposition of the stiffness matrix
D, G = eig(K)

print("Calculating frequencies.")
# calculate the relevant mode information (frequencies, G_inv, damping coefficients)
eigs_pos = np.array([(-(γ*x+η) + np.sqrt((γ*x+η)*(γ*x+η) - (4*x) + 0J))/2 for x in D])
eigs_neg = np.array([(-(γ*x+η) - np.sqrt((γ*x+η)*(γ*x+η) - (4*x) + 0J))/2 for x in D])

freqs = np.array([abs(x)/(2*math.pi) for x in eigs_pos.imag])
damps = abs(eigs_pos.real)
max_damps = max(damps)
damps = np.array([d/max_damps for d in damps])
G_inv = np.linalg.inv(G.real)

bool_array = np.array([x>20 and x<22000 for x in freqs])

freqs = freqs[bool_array]
G_inv = G_inv[bool_array]
damps = damps[bool_array]


print("Calculating modes.")
# write reduced modes
rmodes = np.zeros(G_inv.T.shape)

impulse = np.zeros((len(V)*3,))
impulse.shape = (len(V)*3, 1)
impulse[0] = 1
impulse[1] = 0
impulse[2] = 0

# calculate final gain values and format relevant stuff as list (fgd_final)

gains = G_inv.dot(impulse)
pairs = [(f, abs(g[0])) for f, g in zip(freqs, gains)]

rmodes = np.zeros((len(vertex_weights), len(freqs)))

max_gain = 0
for i in range(len(vertex_weights)):
    v = vertex_weights[i]
    impulse = np.zeros((len(V)*3,))
    impulse.shape = (len(V)*3, 1)
    impulse[0] = v
    impulse[1] = 0
    impulse[2] = 0
    
    gains = G_inv.dot(impulse)
    gains = abs(gains)
    
    m = max(gains)
    if m > max_gain:
        max_gain = m
    
    rmodes[i,:] = gains.T
    
rmodes = rmodes.T
rmodes /= m

def write_reduced(filename):
    with open(filename, 'w') as f:
        f.write(str(len(V))     + "\n")
        f.write(str(len(faces)) + "\n")
        f.write(str(len(freqs)) + "\n")
        for v, w in zip(V, vertex_weights):
            f.write(str(v[0]) + " " + str(v[1]) + " " + str(v[2]) + " " + str(w) + "\n")
        for face in faces:
            f.write(str(face[0]) + " " + str(face[1]) + " " + str(face[2]) + "\n")
        for hz in freqs:
            f.write(str(hz) + "\n")
        for d in damps:
            f.write(str(d) + "\n")
        for row in rmodes:
            for g in row:
                f.write(str(g) + " ")
            f.write("\n")
        f.write("END\n")
       
print("Saving file.") 
write_reduced("modes/" + sys.argv[1][:-4] + ".rmodes")

print("Complete.")