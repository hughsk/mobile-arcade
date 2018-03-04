const HOST = window.location.protocol + '//' + window.location.host
const client = require('socket.io-client')(HOST)

console.log(HOST)

client.once('connect', () => {
  console.log('connected!', client.id)
})