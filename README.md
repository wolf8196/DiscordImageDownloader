# DiscordImageDownloader
 Small and simple app to download and keep in sync images (or any other attachments) from Discord channels.
 
 ## Installation
 
 The app can be run as a usual app (.exe file) or installed as a Windows Service by running "install-as-service.bat" file from command prompt with Admin rights.
 
 ## Configuration
 Configuration is done via "appsettings.json" files.
 
1. Create settings file;
   - Copy/paste default "appsettings.json" file and rename it to "appsettings.production.json";
3. Configure authentication;
   - Open Discord in a browser, open developer tool and go to Network tab;
   - View some channels/scroll some messages until you see a row that starts with "/messages?";
   - Click on the row & find "authorization:" in "Request Headers" group;
   - Copy/paste the value into "Token" part of the settings file from (1);
   - From time to time you will need to redo these steps to update the token; 
4. Configure channels you want to keep in sync;
   - Use "Channels" block to configure the collection of channels that you want to keep in sync;
   - Each block like this configures one channel:
   ```
   {
      "Name": "", // Used for folder name & logging
      "NotBefore": "2000-01-01", //YYYY-MM-DD
      "Mode": "SyncToLast", // "SyncAll"
      "Channel": "",
      "Destination": ""
   }
   ```
   **Name** - name that represents a channel. It is primarily used for logging;
   
   **NotBefore** - date in format YYYY-MM-DD (e.g. 2020-12-31). Any files that where posted before this date will be ignored;
   
   **Mode** - mode of operation; The default mode is "SyncToLast". The app will download everything that it can during first launch and then will only download new files. Most of the time you won't have to change this mode. "SyncAll" mode is needed if there was some error during first download in "SyncToLast". It will check what files already exist, and download everything else. You'll need to switch back to "SyncToLast" once all files are downloaded;
   
   **Channel** - channel Id; It can be obtained from the url - ```https://discord.com/channels/<serverId>/<channelId>```;
 
   **Destination** - path to a directory where the files should be stored (e.g. ```D:\\Images\\Channel1Images```);
5. Start the app
