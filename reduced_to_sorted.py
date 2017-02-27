#!/usr/local/bin/python3

# Read, sort, and write reduced modes files



import numpy as np
import sys

if len(sys.argv) != 3:
    print("Usage: python reader.py in_file num_modes_to_keep")
    exit()

in_file = sys.argv[1]
out_file = in_file[:in_file.find(".")] + "-sorted.rmodes"
num_modes = int(sys.argv[2])

# open and read the file

with open(in_file, "r") as f:
    lines = f.readlines()

    n = int(lines[0])
    t = int(lines[1])
    m = int(lines[2])

    idx = 3

    verts = []
    weights = []
    for i in range(n):
        verts.append([float(x) for x in lines[idx].split()])
        idx += 1

    weights = [x[3] for x in verts]
    verts = [x[:3] for x in verts]

    tris = []
    for i in range(t):
        tris.append([int(x) for x in lines[idx].split()])
        idx += 1

    modes = []
    for i in range(m):
        modes.append(float(lines[idx]))
        idx += 1

    damps = []
    for i in range(m):
        damps.append(float(lines[idx]))
        idx += 1

    G = np.zeros((n, m))

    for i in range(m):
        G[:, i] = [float(x) for x in lines[idx].split()]
        idx += 1

    # n is number of vertices
    # t is number of triangles
    # m is number of modes

    verts = np.array(verts)     # array of vertices
    weights = np.array(weights) # array of weights
    tris = np.array(tris)       # array of triangles

    modes = np.array(modes)     # array of mode frequencies
    damps = np.array(damps)     # array of dampening exponents

    # Each line in G is the gain values for a certain vertex
    # therefore G is m columns wide and n rows tall
    

# sort by a value
inds = damps.argsort() # get the indices of the sorted modes

modes = modes[inds]
damps = damps[inds]
G = G[:, inds]

# get rid of unwanted modes:
if num_modes < m:
    m = num_modes
    modes = modes[:num_modes]
    damps = damps[:num_modes]
    G = G[:,:num_modes]
    
with open(out_file, 'w') as f:
    f.write(str(len(verts)) + "\n")
    f.write(str(len(tris))  + "\n")
    f.write(str(len(modes)) + "\n")
    for v, w in zip(verts, weights):
        f.write(str(v[0])+" "+str(v[1])+" "+str(v[2])+" "+str(w)+"\n")
    for face in tris:
        f.write(str(face[0])+" "+str(face[1])+" "+str(face[2])+"\n")
    for hz in modes:
        f.write(str(hz) + "\n")
    for d in damps:
        f.write(str(d) + "\n")
    for row in G.T:
        for g in row:
            f.write(str(g) + " ")
        f.write("\n")
    f.write("END\n")
