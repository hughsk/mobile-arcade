const fulltilt = require('@hughsk/fulltilt/dist/fulltilt.js')

const HOST = window.location.protocol + '//' + window.location.host + '/player'
const client = require('socket.io-client')(HOST)

console.log(HOST)

client.once('connect', function () {
  console.log('connected!', client.id)
  client.on('client:connect', function (id) {
  })
})

const h2 = document.querySelector('h2')
const h1 = document.querySelector('h1')
var orientation = null

h1.innerHTML = 'running'

window.onerror = function (e) { h1.innerHTML = e.message || e }
  // window.onclick = null
  var baseEuler = null

  h1.innerHTML = 'clicked'
  new FULLTILT.getDeviceOrientation({
    type: 'world'
  }).then(function (controller) {
    orientation = controller
    h1.innerHTML = 'orientation accessed'
  }).catch(function (e) {
    alert(e.message || e)
  })

  var axes = [0, 0]
  var data = {
    id: null,
    type: null,
    xInput: 0,
    yInput: 0,
  }

  draw()
  function draw () {
    requestAnimationFrame(draw)

    if (!orientation) return
    if (!client.id) return

    var euler = orientation.getScreenAdjustedEuler()

    if (!baseEuler) {
      baseEuler = [euler.gamma, euler.beta]
    }

    axes[0] = clamp((euler.gamma - baseEuler[0]) / 50)
    axes[1] = -clamp(((euler.beta - baseEuler[1])) / 50)

    data.id = client.id
    data.type = 'tilt'
    data.xInput = axes[0]
    data.yInput = axes[1]

    client.emit('client:input', data)

    h2.innerHTML = JSON.stringify(data)
  }

  function clamp (a) {
    return Math.max(-1, Math.min(a, +1))
  }