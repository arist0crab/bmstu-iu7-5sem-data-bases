#!/bin/bash

mcs -target:library -r:System.Data.dll -out:SqlCsLib.dll $1

exit 0