#!/bin/bash

echo "Getting started"

npx redoc-cli build https://rattus.azurewebsites.net/swagger/v1/swagger.yaml && \
mv redoc-static.html index.html && \
echo "Changed name from redoc-static.html to index.html" && \
sed -i '7 i \ \ <link rel="icon" type="image/x-icon" href="images/favicon.ico">' index.html && \
echo -e "\nDone!"
