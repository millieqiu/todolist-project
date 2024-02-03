#!/bin/bash

dotnet ef dbcontext scaffold "Name=ConnectionStrings:Database" Microsoft.EntityFrameworkCore.SqlServer -o ./Models -c DBContext -f --no-pluralize

exit 0