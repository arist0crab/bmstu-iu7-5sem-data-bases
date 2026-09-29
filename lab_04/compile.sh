#!/bin/bash

mcs -target:library -r:System.Data.dll -out:$1.dll $1

exit 0