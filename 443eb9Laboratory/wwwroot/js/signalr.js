// import * as signalR from "@microsoft/signalr";
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/hub')
    .withAutomaticReconnect()
    .build();
let httpRequest = new XMLHttpRequest();
let ipAddress;
httpRequest.open('GET', 'http://ip-api.com/json', true);
httpRequest.send();
httpRequest.onreadystatechange = function () {
    if (httpRequest.readyState == 4 && httpRequest.status == 200) {
        ipAddress = JSON.parse(httpRequest.responseText).query;
    }
};
await connection.start();
while (!ipAddress) {
    await new Promise(resolver => setTimeout(resolver, 200));
}
export { connection, ipAddress };
