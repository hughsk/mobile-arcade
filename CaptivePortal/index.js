const HOST = window.location.protocol + '//' + window.location.host
const client = require('socket.io-client')(HOST)

client.once('connect', () => {
  console.log('connected!', client.id)
  client.on('client:connect', (id) => {
  })
})