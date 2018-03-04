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
  console.log('http://localhost:3000/')
})

io.on('connection', (client) => {
  console.log('connected:', client.id)
})