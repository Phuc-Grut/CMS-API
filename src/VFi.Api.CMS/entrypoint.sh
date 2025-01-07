#!/bin/bash
echo "Starting replace ENV:"
printenv | grep APP_ | while read -r line ; do
  key=$(echo $line | cut -d "=" -f1)
  value=$(echo $line | cut -d "=" -f2)
  # Chỉ thay thế trong các file appsettings.json trong thư mục /app
  find /app -name "appsettings*.json" -type f -exec sed -i "s|{$key}|$value|g" {} \;
  echo "Replace $key done: $line"
done
echo "Replace ENV done!"
#run container service
#nginx
echo "Starting server..."
# Execute the CMD provided in the Dockerfile
exec "$@"
