from subprocess import call

filenames = ["fucked-up-wall.obj", "railing.obj", "sierra.obj", "tube.obj", "white-mountains.obj"]

for name in filenames:
    call(["python", "obj_to_modes.py", name])
