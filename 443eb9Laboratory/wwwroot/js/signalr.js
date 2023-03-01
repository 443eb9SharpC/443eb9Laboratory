export const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .build();
connection.start();
