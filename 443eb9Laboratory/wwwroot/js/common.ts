import { connection } from "./signalr.js";

connection.on('createMessage', createMessage)
connection.on("createErrorMessage", createErrorMessage)

let messageBoxes = document.querySelector('.message-boxes') as HTMLDivElement

export async function createMessage(title: string, content: string) {
    let message =
        `<div class="message">
        <h1 class="title">`+ title + `</h1>
        <p class="content">`+ content + `</p>
    </div>`
    messageBoxes.insertAdjacentHTML('beforeend', message)
    let messageElement = messageBoxes.lastElementChild
    messageElement.classList.add('message-enter')
    await new Promise(resolve => setTimeout(resolve, 1000))
    messageElement.classList.remove('message-enter')
    await new Promise(resolve => setTimeout(resolve, 500))
    messageElement.remove()
}

export async function createErrorMessage(title: string, content: string) {
    let errMessage =
        `<div class="error-message">
        <h1 class="title">`+ title + `</h1>
        <p class="content">`+ content + `</p>
    </div>`
    messageBoxes.insertAdjacentHTML('beforeend', errMessage)
    let messageElement = messageBoxes.lastElementChild
    messageElement.classList.add('message-enter')
    await new Promise(resolve => setTimeout(resolve, 1000))
    messageElement.classList.remove('message-enter')
    await new Promise(resolve => setTimeout(resolve, 500))
    messageElement.remove()
}