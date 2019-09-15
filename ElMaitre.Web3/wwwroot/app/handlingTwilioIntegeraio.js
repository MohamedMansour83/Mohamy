$(function () {
    jQuery(".remotevideos").hide();
    var $ = function (selector, el) {
        if (!el) el = document;
        return el.querySelector(selector);
    }
    var trace = function (what, obj) {
        //var pre = document.createElement("pre");
        //pre.textContent = JSON.stringify(what) + " - " + JSON.stringify(obj || "");
        //$("#immediate").appendChild(pre);

         // console.log(JSON.stringify(what) + " - " + JSON.stringify(obj || ""))
    };
    var main = (function () {
         
        var sessionId;
        var broker;
        var rtc;
        var dc;
        var pathArray = window.location.pathname.split('/');
        sessionId = pathArray[pathArray.length - 1];
        trace("Ready");
        trace("Try connect the connectionBroker");
        var ws = new XSockets.WebSocket("wss://rtcplaygrouund.azurewebsites.net:443", ["connectionbroker"], {
            ctx: sessionId
        });
        var onError = function (err) {
            trace("error", arguments);
        };
        var recordMediaStream = function (stream) {
            if ("MediaRecorder" in window === false) {
                trace("Recorder not started MediaRecorder not available in this browser. ");
                return;

            }
            var recorder = new XSockets.MediaRecorder(stream);
            recorder.start();
            trace("Recorder started.. ");
            recorder.oncompleted = function (blob, blobUrl) {
                trace("Recorder completed.. ");
                var li = document.createElement("li");
                var download = document.createElement("a");
                download.textContent = new Date();
                download.setAttribute("download", XSockets.Utils.randomString(8) + ".webm");
                download.setAttribute("href", blobUrl);
                li.appendChild(download);
                $("ul").appendChild(li);

            };
        };
        var addRemoteVideo = function (peerId, mediaStream) {
            var remoteVideo = document.createElement("video");
            remoteVideo.setAttribute("autoplay", "autoplay");
            remoteVideo.setAttribute("controls", true);
            remoteVideo.setAttribute("rel", peerId);
            attachMediaStream(remoteVideo, mediaStream);
            jQuery(".remotevideos").html(remoteVideo);
            jQuery(".remotevideos").show();

        };
        var onConnectionLost = function (remotePeer) {
            trace("onconnectionlost", arguments);
            var peerId = remotePeer.PeerId;
            var videoToRemove = $("video[rel='" + peerId + "']");
            jQuery(".remotevideos").html("");
            jQuery(".remotevideos").hide();

            jQuery("#chat").hide();
            jQuery("#notify-modal").modal('show');
        };
        var oncConnectionCreated = function () {
             // console.log(arguments, rtc);
            trace("oncconnectioncreated", arguments);
            jQuery("#chat").show();
        };
        var onGetUerMedia = function (stream) {
            trace("Successfully got some userMedia , hopefully a goat will appear..");
            rtc.connectToContext(); // connect to the current context?
        };
        var onRemoteStream = function (remotePeer) {
             
            addRemoteVideo(remotePeer.PeerId, remotePeer.stream);
            trace("Opps, we got a remote stream. lets see if its a goat..");
        };
        var onLocalStream = function (mediaStream) {
            trace("Got a localStream", mediaStream.id);
            attachMediaStream($(".mediaplayer video "), mediaStream);
            // if user click, video , call the recorder
            $(".mediaplayer video ").addEventListener("click", function () {
                recordMediaStream(rtc.getLocalStreams()[0]);
            });
        };
        var onContextCreated = function (ctx) {
            trace("RTC object created, and a context is created - ", ctx);
            rtc.getUserMedia(rtc.userMediaConstraints.hd(true), onGetUerMedia, onError);
        };
        var onOpen = function () {
             
            trace("Connected to the brokerController - 'connectionBroker'");
            rtc = new XSockets.WebRTC(this);

            rtc.onlocalstream = onLocalStream;
            rtc.oncontextcreated = onContextCreated;
            rtc.onconnectioncreated = oncConnectionCreated;
            rtc.onconnectionlost = onConnectionLost;
            rtc.onremotestream = onRemoteStream;


            rtc.onanswer = function (event) {

            };

            rtc.onoffer = function (event) {

            };

            dc = new XSockets.WebRTC.DataChannel("chat");

            rtc.addDataChannel(dc);


            var peerIdToSend;
            dc.onopen = function (peerId, event) {
                peerIdToSend = peerId;
                // peerId is the identity of the PeerConnection
                // event is the RTCDataChannel (native) event



            };

            dc.onclose = function (peerId) {
                // peerId is the identity of the PeerConnection
                // event is the RTCDataChannel (native) event
            };

            dc.subscribe("foo", function (message) {
                var el = jQuery(".temp_base_receive").clone();
                el.css("display", "flex");
                el.removeClass("temp_base_receive");
                el.find(".msg_receive p").text(message.who);
                jQuery(".msg_container_base").append(el);
            });



            dc.subscribe("fileshare", function (file) {
                var blob = new Blob([file.binary], {
                    type: file.data.type
                });
                var download = jQuery("<a>").text(file.data.name).attr({
                    download: file.data.filename,
                    href: URL.createObjectURL(blob),
                    target: "_blank"
                });
                // do op's with the download element
                var el = jQuery(".temp_base_receive").clone();
                el.css("display", "flex");
                el.removeClass("temp_base_receive");
                el.find(".msg_receive p").html(download);
                jQuery(".msg_container_base").append(el);
            });

            dc.onpublish = function (topic, data) {
                // do op
                alert(topic);
            };

            $("#btn-chat").addEventListener("click", function () {




                if (jQuery("#file").val()) {

                    var fileByteArray = [];
                    var file = jQuery("#file").get(0).files[0];
                    var reader = new FileReader();
                    reader.readAsArrayBuffer(file);
                    reader.onload = function (e) {
                        var theBytes = e.target.result;
                        var bytes = new Uint8Array(theBytes);


                        //var b = ab2str(theBytes);
                        // // console.log(b);

                        //dc.publishTo(peerIdToSend, "file-bytes", {
                        //    who: b,
                        //    name: file.name,
                        //    type: file.type,
                        //    size: file.size
                        //});


                        dc.publishBinary("fileshare", bytes, {
                            name: file.name,
                            type: file.type,
                            size: file.size
                        });

                        var blob = new Blob([bytes], {
                            type: file.type
                        });

                        var download = jQuery("<a>").text(file.name).attr({
                            download: file.filename,
                            href: URL.createObjectURL(blob),
                            target: "_blank"
                        });

                        var el = jQuery(".temp_base_sent").clone();
                        el.css("display", "flex");
                        el.removeClass("temp_base_sent");
                        el.find(".msg_sent p").html(download);
                        jQuery(".msg_container_base").append(el);
                        jQuery("#btn-input").val('');






                    }

                    jQuery("#file").val('');


                } else if (jQuery("#btn-input").val().length > 0) {
                    dc.publishTo(peerIdToSend, "foo", { who: jQuery("#btn-input").val() });

                    var el = jQuery(".temp_base_sent").clone();
                    el.css("display", "flex");
                    el.removeClass("temp_base_sent");
                    el.find(".msg_sent p").text(jQuery("#btn-input").val());
                    jQuery(".msg_container_base").append(el);
                    jQuery("#btn-input").val('');
                }
            });

            $("#btn-input").addEventListener("keypress", function (e) {
                var keyCode = e.keyCode;
                if (keyCode == 13) {
                    $("#btn-chat").click();
                }
            }, false);
        };
        function ab2str(buf) {
            // return String.fromCharCode.apply(null, new Uint8Array(buf));

            var uintArray = new Uint8Array(buf);
            var converted = [];
            uintArray.forEach(function (byte) { converted.push(String.fromCharCode(byte)) });
            return converted;
        }
        function str2ab(str) {
            var buf = new ArrayBuffer(str.length * 2); // 2 bytes for each char
            var bufView = new Uint8Array(buf);
            for (var i = 0, strLen = str.length; i < strLen; i++) {
                bufView[i] = str.charCodeAt(i);
            }
            return buf;
        }
        var onConnected = function () {
            trace("connection to the 'broker' server is established");
            trace("Try get the broker controller form server..");
            broker = ws.controller("connectionbroker");
            broker.onopen = onOpen;
        };
        ws.onconnected = onConnected;
    });
    main();

});

$("header").removeClass("navbar-dark");
$("#menu_navbar").addClass("shadow");
$("#top_bar").addClass("shadow-sm");

function openForm() {
    document.getElementById("myForm").style.display = "block";
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
}
