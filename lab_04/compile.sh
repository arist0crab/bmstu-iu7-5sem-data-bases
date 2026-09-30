#!/bin/bash

mcs -target:library -r:System.Data.dll -out:release/$1.dll $1.cs

exit 0