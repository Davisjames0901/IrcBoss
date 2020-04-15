dotnet publish ../src/Asperand.IrcBallistic.Worker

if [ -f /etc/systemd/system/digbot.service ]; then
  echo "Service exists!"
  sudo systemctl stop digbot.service
  echo "Service stopped"
else
  echo "Service not found! Creating..."
  sudo cp digbot.service /etc/systemd/system/
  echo "Created!"
fi
if [ -d /var/opt/digbot ]; then
  echo "Deleting old files"
  sudo rm -r /var/opt/digbot
fi
echo "Copying files..."
sudo cp -r ../src/Asperand.IrcBallistic.Worker/bin/Release/netcoreapp3.1 /var/opt/digbot

echo "Done! Starting service!"
sudo systemctl start digbot.service
