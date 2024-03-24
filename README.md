# Docker Compose Training

This repository contains training material for docker compose. It uses a dotnet webapp, mongo, redis, and nginx.

This material is meant to be consumed as a live demonstration, but can be used for self-study.

⚠ This is written for use on a linux machine. If you're using WSL docker, the networking can be a bit weird but the concepts are the same. ⚠

## Prerequisites

1. Understand how to use the docker cli.
1. Understand how to write a Dockerfile.

## Learning Objectives

1. Understand how to combine multiple services into a single docker-compose file.
1. Understand how to both build and fetch images in a docker-compose file.
1. Understand how to add persistence volumes to these services.
1. Understand how to use docker networking, both internal and external.
1. Understand how to add a reverse proxy to your docker-compose stack.
1. Understand how to add load balancing to your docker-compose stack.

## Using this Repository

I have tried my best to break this down into small, digestible chunks. Each chunk is in its own branch. You can use the following commands to navigate between branches:

```bash
git switch stage-x
```

Where `x` is the stage number you want to view.

Each stage has a `speaker-notes.md` file that contains the notes I would use if I were presenting this material. You can use this to follow along with the material.

The PRs for each stage are also a potentially useful reference point, but I did have to backport fixes to mistakes as I went to the various branches, so they aren't perfect.
