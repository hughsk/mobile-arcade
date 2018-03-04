const express = require('express')
const socketio = require('socket.io')
const path = require('path')
const http = require('http')

const app = express()
const server = http.createServer(app)
const io = socketio(server)

app.use(express.static(path.resolve(__dirname, 'static')))

server.listen(3000, (err) => {
  if (err) throw err
})

const channelPlayer = io.of('/player')

channelPlayer.on('connection', (client) => {
  io.emit('client:connect', client.id)
  channelHost.emit('client:connect', client.id)
  client.on('disconnect', () => {
    io.emit('client:disconnect', client.id)
  })
})

io.on('connection', (client) => {
  console.log("HOST CONNECTED", client.id, client.nsp.name)
})