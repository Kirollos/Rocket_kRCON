/*
	Copyright 2015 Kirollos

	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

var http = require('http');
var fs = require('fs');

//var port = process.argv[2]; // Had it for testing purposes, not needed anymore.

var httpsvr = http.createServer(function(req, resp) {

	var f,t;

	if(req.url == "/") {
		f = "index.html";
		t = "text/html";
	}
	else if(req.url == "/CONFIG") {
		f = "../config.js";
		t = "application/javascript";
	}
	else {
		f = req.url;
		if(f.match('.html'))
			t = "text/html";
		if(f.match('.css'))
			t = "text/css";
		if(f.match('.js'))
			t = "application/javascript";
	}
	try {
		var fresp = fs.readFileSync("web/"+f);

		resp.writeHead(200, {"Content-type": t});
		resp.end(fresp);
	}
	catch(e) {
		resp.writeHead(404);
		resp.end("File not found.");
	}
});
var sock = require('socket.io')(httpsvr);

var _config = require('./config.js').config;

sock.on('connection', function(socket){
	var rcon = new require('net').Socket();
	console.log("New connection from " + socket.handshake.address);
	rcon.connect(_config.krcon_port, _config.krcon_ip, function(){
		rcon.write("set redrawcmd false\r\n"); // redrawing only works on actual consoles/terminals
	});
	rcon.on('data', function(data){
		socket.emit('on-receive', {resp: data.toString('utf8').trim()});
	});
	rcon.on('close', function(data){
		socket.emit('rip');
		if(rcon != null) {
			rcon.destroy();
		}
		rcon = null;
	});
	socket.on('on-send', function(data){
		console.log("Client ("+socket.handshake.address+") has executed command '"+data.cmd+"'");
		rcon.write(data.cmd + "\r\n");
	});
	socket.on('disconnect', function(data){
		if(rcon != null) {
			rcon.destroy();
		}
		console.log("Disconnected from " + socket.handshake.address);
		rcon = null;
	});
});


httpsvr.listen(_config.webhost_port);