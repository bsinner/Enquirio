#!/bin/bash

file="/home/blake/Projects/questions/DesignDocuments/databaseDesign.sql"

mysql --user="enquirio" --password --execute="USE enquirio; source $file; USE enquirio_test; source $file;"