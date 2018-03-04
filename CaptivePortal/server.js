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

io.on('connection', (client) => {
  io.emit('client:connect', client.id)
  client.on('disconnect', () => {
    io.emit('client:disconnect', client.id)
  })
})