import { Component, ElementRef, ViewChild , Input, Output, EventEmitter } from '@angular/core';
import { OnInit, AfterViewInit, Renderer2  } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
//const { connect } = require('twilio-video');
import { connect, ConnectOptions, Room, createLocalTracks, LocalTrack, LocalVideoTrack, RemoteVideoTrackPublication } from 'twilio-video';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { TokenModel } from '../models/TokenModel';
import { Pipe, PipeTransform } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  //title = 'app1';
  //videoTrack: LocalVideoTrack;
  //isInitializing: boolean = true;
  //private localTracks: LocalTrack[] = [];
  //@ViewChild('preview') previewElement: ElementRef;
  //@Input('isPreviewing') isPreviewing: boolean;
  //@Output() settingsChanged = new EventEmitter<MediaDeviceInfo>();
  //private videoDeviceId: string;
  private sessionId: string;
  //private err: string;
  //private top: number = 0;
  //private devices: MediaDeviceInfo[] = [];
  //@ViewChild('camera') camera: CameraComponent;
  //@ViewChild('videoSelect') video: DeviceSelectComponent;

  constructor(
    private readonly renderer: Renderer2, private http: HttpClient, private activatedRoute: ActivatedRoute
    , private router: Router) {
     // //console.log(router.parseUrl(router.url));
    }

  async ngOnInit() {
    let params: any = this.activatedRoute.snapshot.params;
     // //console.log(params.id);
    this.activatedRoute.queryParams.subscribe(params => {
      const userId = params['userId'];
       // //console.log(userId);
    });
    await this.getIdFromUrl();
    //await this.getIdFromUrl2();
  }

  //async getIdFromUrl2() {
  //  let that = this;
  //  await this.activatedRoute.params.subscribe(params => {
  //    that.sessionId = params["id"];
  //     // //console.log(params);
  //     // //console.log(that.sessionId);
  //  });
  //}

  async getIdFromUrl() {
    let that = this;
    await this.activatedRoute.queryParams.subscribe(params => {
      that.sessionId = params['id'];
       // //console.log(that.sessionId);
       // //console.log(params);
      if (that.sessionId !== undefined) {
        //that.router.navigateByUrl('/videomohamy/' + that.sessionId);
      }

    });
  }

  //async ngAfterViewInit() {
  //  let that = this;
  //  //this.activatedRoute.queryParams.subscribe(params => {
  //  //  //const sessionId = params['sessionId'];
  //  //  that.sessionId = params['sessionId'];
  //  //   // //console.log(that.sessionId);
  //  //});

  //  await this.getIdFromUrl();
  //  if (this.sessionId != null) {
  //    await this.checkTheSession();
  //    if (this.err == null) {
  //      if (this.previewElement && this.previewElement.nativeElement) {
  //        // // //console.log('gendy');
  //        await this.initializeDevice();
  //        this.loadRoom();
  //      }
  //    }
  //  }
  //  else {
  //    alert('no session');
  //  }
  //}

  //async onSettingsChanged(deviceInfo: MediaDeviceInfo) {
  //  if (this.isPreviewing) {
  //    await this.showPreviewCamera();
  //  } else {
  //    this.settingsChanged.emit(deviceInfo);
  //  }
  //}

  //async showPreviewCamera() {
  //  this.isPreviewing = true;

  //  //if (this.videoDeviceId !== this.video.selectedId) {
  //  //  this.videoDeviceId = this.video.selectedId;
  //  //  const videoDevice = this.devices.find(d => d.deviceId === this.video.selectedId);
  //  //  await this.camera.initializePreview(videoDevice);
  //  //}

  //  //return this.camera.tracks;

  //  await this.initializePreview(this.devices[0]);
  //}

  //initializePreview(deviceInfo?: MediaDeviceInfo) {
  //   // //console.log(deviceInfo);  
  //  if (deviceInfo) {
  //    this.initializeDevice(deviceInfo.kind, deviceInfo.deviceId);
  //  } else {
  //    this.initializeDevice();
  //  }
  //}


  //finalizePreview() {
  //  try {
  //    if (this.videoTrack) {
  //      this.videoTrack.detach().forEach(element => element.remove());
  //    }
  //  } catch (e) {
  //    console.error(e);
  //  }
  //}

  //private async initializeDevice(kind?: MediaDeviceKind, deviceId?: string) {
  //  try {
  //     // //console.log(kind);
  //     // //console.log(deviceId);
  //    this.isInitializing = true;

  //    //this.finalizePreview();

  //    this.localTracks = kind && deviceId
  //      ? await this.initializeTracks(kind, deviceId)
  //      : await this.initializeTracks();

  //    this.videoTrack = this.localTracks.find(t => t.kind === 'video') as LocalVideoTrack;
  //    const videoElement = this.videoTrack.attach();
  //    this.renderer.setStyle(videoElement, 'position', 'absolute');
  //    this.renderer.setStyle(videoElement, 'top', this.top + 'px');
  //    this.renderer.setStyle(videoElement, 'height', '250px');
  //    this.renderer.setStyle(videoElement, 'width', '100%');
  //    this.renderer.appendChild(this.previewElement.nativeElement, videoElement);
  //  } finally {
  //    this.isInitializing = false;
  //  }
  //}

  //private initializeTracks(kind?: MediaDeviceKind, deviceId?: string) {
  //  if (kind) {
  //    switch (kind) {
  //      case 'audioinput':
  //        return createLocalTracks({ audio: { deviceId }, video: true });
  //      case 'videoinput':
  //        return createLocalTracks({ audio: true, video: { deviceId } });
  //    }
  //  }

  //  return createLocalTracks({ audio: true, video: true });
  //}

  //stringGen(len) {
  //  var text = "";

  //  var charset = "abcdefghijklmnopqrstuvwxyz0123456789";

  //  for (var i = 0; i < len; i++)
  //    text += charset.charAt(Math.floor(Math.random() * charset.length));

  //  return text;
  //}

  
  //loadRoom() {
  //  var url = `${environment.baseUrl}ChatVideo/Tokenization2/` + this.sessionId + "?id2=" + this.stringGen(8);

  //  let requestHeaders = new httpHeaders();
  //  requestHeaders = requestHeaders.set('Access-Control-Allow-Origin', '*');
  //  requestHeaders = requestHeaders.set('Access-Control-Allow-Methods', 'GET, POST, DELETE, PUT');
  //  requestHeaders = requestHeaders.set('Access-Control-Allow-Headers', 'Origin, Content-Type, X-Auth-Token');
  //  let that = this;
  //  this.Http.get(url, { headers: requestHeaders }).subscribe(data => {
  //    var obj = (data as TokenModel);
  //    connect(obj.token,
  //      { name: that.sessionId + "_room" }).then(room => {
  //         // //console.log(`Successfully joined a Room: ${room}`);
  //        // // //console.log(room);


  //        room.participants.forEach(participant1 => {

  //          //that.initializeDevice("videoinput", participant.identity, "300px");

  //           // //console.log(`Participant "${participant1.identity}" is connected to the Room`);

  //          participant1.on('trackSubscribed', function (track1) {


  //            if (track1.kind === 'video') {

  //              // // //console.log(track1.isStarted);
  //              that.top = that.top + 300;
  //              // // //console.log(track1.dimensions.height);
  //              // // //console.log(track1.mediaStreamTrack);
  //              const videoElement2 = track1.attach();
  //               // //console.log(videoElement2.paused);
  //              that.renderer.setStyle(videoElement2, 'position', 'absolute');
  //              that.renderer.setStyle(videoElement2, 'top', that.top + 'px');
  //              that.renderer.setStyle(videoElement2, 'height', '250px');
  //              that.renderer.setStyle(videoElement2, 'width', '100%');
  //              that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);

  //            }

  //          });

  //          // 
  //          //participant.videoTracks.forEach(videoTrack =>  // //console.log(videoTrack));

  //        });


  //        //room.localParticipant.forEach(participant => {

  //        //  //that.initializeDevice("videoinput", participant.identity, "300px");

  //        //   // //console.log(`Participant "${participant.identity}" is connected to the Room`);

            

  //        //  // 
  //        //  //participant.videoTracks.forEach(videoTrack =>  // //console.log(videoTrack));

  //        //});

  //        //room.localParticipant.on('trackSubscribed', function (track) {

  //        //   // //console.log(track);

  //        //  if (track.kind === 'video') {
  //        //     // //console.log(track.kind);
  //        //    const videoElement2 = track.attach();
  //        //     // //console.log(videoElement2);
  //        //    that.renderer.setStyle(videoElement2, 'position', 'absolute');
  //        //    that.renderer.setStyle(videoElement2, 'top', '0px');
  //        //    that.renderer.setStyle(videoElement2, 'height', '250px');
  //        //    that.renderer.setStyle(videoElement2, 'width', '100%');
  //        //    that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);
  //        //  }

  //        //});

  //        //room.participants.forEach(participant => {

  //        //  //that.initializeDevice("videoinput", participant.identity, "300px");

  //        //   // //console.log(`Participant "${participant.identity}" is connected to the Room`);

  //        //  participant.on('trackSubscribed', function (track) {

  //        //    // // //console.log(track);

  //        //    if (track.kind === 'video') {
  //        //      if (track.isStarted) {
  //        //         // //console.log(track.kind);
  //        //         // //console.log(that.top);
  //        //        const videoElement2 = track.attach();
  //        //         // //console.log(videoElement2);
  //        //        that.renderer.setStyle(videoElement2, 'position', 'absolute');
  //        //        that.renderer.setStyle(videoElement2, 'top', that.top + 'px');
  //        //        that.renderer.setStyle(videoElement2, 'height', '250px');
  //        //        that.renderer.setStyle(videoElement2, 'width', '100%');
  //        //        that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);
  //        //        that.top = that.top + 300;
  //        //      }
                
  //        //      }

  //        //  });

  //        //  // 
  //        //  //participant.videoTracks.forEach(videoTrack =>  // //console.log(videoTrack));

  //        //});


  //        room.on('participantConnected', participant => {
  //           // //console.log(`A remote Participant connected: ${participant}`);
  //          // // //console.log(participant);

  //          //participant.on('trackSubscribed', function (track) {

  //          //   // //console.log(track);

  //          //  if (track.kind === 'video') {
  //          //    that.top = that.top + 300;
  //          //     // //console.log(track.kind);
  //          //    const videoElement2 = track.attach();
  //          //     // //console.log(videoElement2);
  //          //    that.renderer.setStyle(videoElement2, 'position', 'absolute');
  //          //    that.renderer.setStyle(videoElement2, 'top', that.top + 'px');
  //          //    that.renderer.setStyle(videoElement2, 'height', '250px');
  //          //    that.renderer.setStyle(videoElement2, 'width', '100%');
  //          //    that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);
  //          //  }

  //          //});

  //          room.participants.forEach(participant1 => {

  //            //that.initializeDevice("videoinput", participant.identity, "300px");

  //             // //console.log(`Participant "${participant1.identity}" is connected to the Room`);

  //            participant1.on('trackPublished', track => {
  //               // //console.log(`Participant "${participant1.identity}" published ${track.kind} Track ${track.sid}`);
  //              if (track.kind === 'data') {
  //                track.on('message', data => {
  //                   // //console.log(data);
  //                });
  //              }
  //            });

  //            //participant1.on('trackRemoved', track => {
  //            //   // //console.log(`Participant "${participant1.identity}" removed ${track.kind} Track ${track.sid}`);
  //            //  if (track.kind === 'data') {
  //            //    track.on('message', data => {
  //            //       // //console.log(data);
  //            //    });
  //            //  }
  //            //});

  //            participant1.on('trackSubscribed', function (track1) {


  //              if (track1.kind === 'video') {

  //                // // //console.log(track1.isStarted);
  //                that.top = that.top + 300;
  //                // // //console.log(track1.dimensions.height);
  //                // // //console.log(track1.mediaStreamTrack.muted);
  //                const videoElement2 = track1.attach();
  //                 // //console.log(videoElement2.paused);
  //                that.renderer.setStyle(videoElement2, 'position', 'absolute');
  //                that.renderer.setStyle(videoElement2, 'top', that.top + 'px');
  //                that.renderer.setStyle(videoElement2, 'height', '250px');
  //                that.renderer.setStyle(videoElement2, 'width', '100%');
  //                that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);
                  
  //              }

  //            });

  //            participant1.on('trackUnsubscribed', function (track1) {
  //               // //console.log('gendy');
  //            });

  //            // 
  //            //participant.videoTracks.forEach(videoTrack =>  // //console.log(videoTrack));

  //          });

  //          //participant.on('trackSubscribed', function (track) {

  //          //   // //console.log(track);

  //          //  if (track.kind === 'video') {
  //          //     // //console.log(track.kind);
  //          //    const videoElement2 = track.attach();
  //          //     // //console.log(videoElement2);
  //          //    that.renderer.setStyle(videoElement2, 'position', 'absolute');
  //          //    that.renderer.setStyle(videoElement2, 'top', '250px');
  //          //    that.renderer.setStyle(videoElement2, 'height', '250px');
  //          //    that.renderer.setStyle(videoElement2, 'width', '100%');
  //          //    that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);
  //          //  }

  //          //});

  //          //participant.videoTracks.forEach(videoTrack => {
  //          //  // // //console.log(videoTrack);
  //          //  ////var videoTrack2 = videoTrack as RemoteVideoTrackPublication;
  //          //  //// // //console.log(videoTrack2);
  //          //  // // //console.log(videoTrack.track);
  //          //  // // //console.log(videoTrack.trackSid);
  //          //  //const videoElement2 = videoTrack.track.attach();
  //          //  //that.renderer.setStyle(videoElement2, 'position', 'absolute');
  //          //  //that.renderer.setStyle(videoElement2, 'top', top ? top : '250px');
  //          //  //that.renderer.setStyle(videoElement2, 'height', '250px');
  //          //  //that.renderer.setStyle(videoElement2, 'width', '100%');
  //          //  //that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);

  //          //});

  //          //participant.createLocalTracks({ audio: true, video: `${participant.identity}` });
  //          //that.initializeDevice("videoinput", participant.identity, "300px");

            

  //        });
  //      }, error => {
  //        console.error(`Unable to connect to Room: ${error.message}`);
  //      });
  //  });

  //  //var token = 'eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiIsImN0eSI6InR3aWxpby1mcGE7dj0xIn0.eyJqdGkiOiJTSzZlNWM5MzgyOWUxODcxMzUzNDA5MzgwZWRkZjQ1NjQzLTE1NTg5NjE0MjkiLCJpc3MiOiJTSzZlNWM5MzgyOWUxODcxMzUzNDA5MzgwZWRkZjQ1NjQzIiwic3ViIjoiQUM0MmIxZDMyM2IxMmViY2ZkNzUwZDFkZTY4MWQ0N2Y4MCIsImV4cCI6MTU1ODk2NTAyOSwiZ3JhbnRzIjp7ImlkZW50aXR5IjoiQUM0MmIxZDMyM2IxMmViY2ZkNzUwZDFkZTY4MWQ0N2Y4MCIsInZpZGVvIjp7InJvb20iOiJteS1uZXctcm9vbSJ9fX0.bEO-wZDeCEoE1kwBEJPgx2TacFzCZ-EsT3AAwM6YleY';
  //  //connect(token,
  //  //  { name: 'my-new-room' }).then(room => {
  //  //   // //console.log(`Successfully joined a Room: ${room}`);
  //  //  room.on('participantConnected', participant => {
  //  //     // //console.log(`A remote Participant connected: ${participant}`);
  //  //    room.participants.forEach(participant => {
  //  //       // //console.log(`Participant "${participant.identity}" is connected to the Room`);
  //  //    });   

  //  //  });
  //  //}, error => {
  //  //  console.error(`Unable to connect to Room: ${error.message}`);
  //  //});
  //}


  //async checkTheSession() {
  //  var url = `${environment.baseUrl}api/SessionApi/CheckSession/` + this.sessionId;

  //  let requestHeaders = new httpHeaders();
  //  requestHeaders = requestHeaders.set('Access-Control-Allow-Origin', '*');
  //  requestHeaders = requestHeaders.set('Access-Control-Allow-Methods', 'GET, POST, DELETE, PUT');
  //  requestHeaders = requestHeaders.set('Access-Control-Allow-Headers', 'Origin, Content-Type, X-Auth-Token');
  //  let that = this;
  //  await this.Http.get(url, { headers: requestHeaders }).toPromise()
  //    .catch(err => this.handleError(err));
  //}

  //handleError(error) {
  //   // //console.log(error);
  //  let errorMessage = '';
  //  if (error.error instanceof ErrorEvent) {
  //    // client-side error
  //    errorMessage = `Error: ${error.error.message}`;
  //  } else {
  //    // server-side error
  //    errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
  //  }
  //  window.alert(errorMessage);
  //  this.err = errorMessage;
  //  return throwError(errorMessage);
  //}
}
