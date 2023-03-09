// import * as signalR from "@microsoft/signalr";
const connection = new signalR.HubConnectionBuilder()
    .withUrl('/hub')
    .withAutomaticReconnect()
    .build();
let httpRequest = new XMLHttpRequest();
let ipAddress;
httpRequest.open('GET', 'https://ip.useragentinfo.com/json', true);
httpRequest.send();
httpRequest.onreadystatechange = function () {
    if (httpRequest.readyState == 4 && httpRequest.status == 200) {
        ipAddress = JSON.parse(httpRequest.responseText).ip;
    }
};
await connection.start();
while (!ipAddress) {
    await new Promise(resolver => setTimeout(resolver, 200));
}
export { connection, ipAddress };
