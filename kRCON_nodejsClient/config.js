var kRCON_Config = 
{
	webhost_ip:		"127.0.0.1",	// The IP of the server hosting the node.js script (Don't set it to localhost if you are hosting it on a public server)
	webhost_port:	27014,			// The Port the webserver will be running on
	krcon_ip:		"127.0.0.1",	// The IP of the kRCON connection
	krcon_port:		27015			// The port of the kRCON connection
};


// Don't touch..
if(typeof module != 'undefined')
{
	module.exports.config = kRCON_Config;
}