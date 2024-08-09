# PostCreator
# Step 1 (change the host file)

Update host file on your PC like this instruction https://www.nublue.co.uk/guides/edit-hosts-file/#:~:text=In%20Windows%2010%20the%20hosts,%5CDrivers%5Cetc%5Chosts.

Need to path these lines

127.0.0.1 www.postcreator.com
0.0.0.0 www.postcreator.com
192.168.0.4 www.postcreator.com
# Step 2 (download node.js)
1. The download url: https://nodejs.org/en
# Step 3 (download angular cli)
1. To open the cmd
2. Command: npm install -g @angular/cli
# Step 4 (Build the angular app)
1. To open cmd
2. Select path ./PostCreator/Web/post-web
3. To type command "npm run build"
# Step 5 (to start docker)
1. To open cmd
2. Select path of the project "PostCreator"
3. docker-compose up
