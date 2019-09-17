import { Component, ElementRef, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { OnInit, AfterViewInit, Renderer2 } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
//const { connect } = require('twilio-video');
import { connect, ConnectOptions, Room, createLocalTracks, LocalTrack, LocalVideoTrack, RemoteVideoTrackPublication } from 'twilio-video';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';
import { TokenModel } from '../../models/TokenModel';
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
import { LocalStorageService } from '../services/local-storage.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-videomohamy',
  templateUrl: './videomohamy.component.html',
  styleUrls: ['./videomohamy.component.css']
})
export class VideoMohamyComponent implements OnInit {
  activeRoom;
  title = 'app1';
  videoTrack: LocalVideoTrack;
  isInitializing: boolean = true;
  private localTracks: LocalTrack[] = [];
  @ViewChild('preview') previewElement: ElementRef;
  @Input('isPreviewing') isPreviewing: boolean;
  @Output() settingsChanged = new EventEmitter<MediaDeviceInfo>();
  private videoDeviceId: string;
  sessionId: string;
  isClicked: boolean = false;
  isLoading: boolean = false;
  private err: string;
  sessionkey: string;
  startSession: string;
  private top: number = 0;
  private devices: MediaDeviceInfo[] = [];
  //@ViewChild('camera') camera: CameraComponent;
  //@ViewChild('videoSelect') video: DeviceSelectComponent;

  constructor(
    private readonly renderer: Renderer2, private http: HttpClient,
    private activatedRoute: ActivatedRoute, private router: Router,
    private localStorageService: LocalStorageService, private cookieService: CookieService) { }

  async ngOnInit() {
     
    this.sessionkey = "Please Enter Your Session Key";
    this.startSession = "Start Session";
    //await this.getIdFromUrl2();
    await this.getIdFromUrl();
    if (this.sessionId != "") {
      await this.onBtnClicked();
    }
    //window.addEventListener('beforeunload', this.leaveRoom);
  }

  async getIdFromUrl2() {
    let that = this;
    await this.activatedRoute.params.subscribe(params => {
      that.sessionId = params["id"];
      //console.log(params);
      //console.log(that.sessionId);
    });
  }

  async getIdFromUrl() {
    let that = this;
    await this.activatedRoute.queryParams.subscribe(params => {
      that.sessionId = params['id'];
    });
  }

  async ngAfterViewInit() {
    let that = this;
    //this.activatedRoute.queryParams.subscribe(params => {
    //  //const sessionId = params['sessionId'];
    //  that.sessionId = params['sessionId'];
    //  //console.log(that.sessionId);
    //});

    //await this.getIdFromUrl2();
    ////await this.getIdFromUrl();
    //if (this.sessionId != null) {
    //  await this.checkTheSession();
    //  if (this.err == null) {
    //    if (this.previewElement && this.previewElement.nativeElement) {
    //      ////console.log('gendy');
    //      await this.initializeDevice();
    //      this.loadRoom();
    //    }
    //  }
    //}
    //else {
    //  //alert('no session');
    //}
  }

  async onBtnClicked() {
     
    this.isLoading = true;
    await this.checkTheSession();
    this.isLoading = false;
    this.isClicked = true;
    if (this.err == null) {
      if (this.previewElement && this.previewElement.nativeElement) {
        await this.initializeDevice();
        this.loadRoom();
        
      }
      else {
        //console.log('ss');
      }
    }
  }

  async onSettingsChanged(deviceInfo: MediaDeviceInfo) {
    if (this.isPreviewing) {
      await this.showPreviewCamera();
    } else {
      this.settingsChanged.emit(deviceInfo);
    }
  }

  async showPreviewCamera() {
    this.isPreviewing = true;

    await this.initializePreview(this.devices[0]);
  }

  initializePreview(deviceInfo?: MediaDeviceInfo) {
    //console.log(deviceInfo);
    if (deviceInfo) {
      this.initializeDevice(deviceInfo.kind, deviceInfo.deviceId);
    } else {
      this.initializeDevice();
    }
  }

  finalizePreview() {
    try {
      if (this.videoTrack) {
        this.videoTrack.detach().forEach(element => element.remove());
      }
    } catch (e) {
      console.error(e);
    }
  }

  private async initializeDevice(kind?: MediaDeviceKind, deviceId?: string) {
    try {
      this.isInitializing = true;
      this.localTracks = kind && deviceId
        ? await this.initializeTracks(kind, deviceId)
        : await this.initializeTracks();
      this.videoTrack = this.localTracks.find(t => t.kind === 'video') as LocalVideoTrack;
      const videoElement = this.videoTrack.attach();
      this.renderer.setStyle(videoElement, 'position', 'absolute');
      this.renderer.setStyle(videoElement, 'left', '30px');
      this.renderer.setStyle(videoElement, 'bottom', '50px');
      this.renderer.setStyle(videoElement, 'max-width', '340px');
      this.renderer.setStyle(videoElement, 'box-sizing', 'border-box');
      this.renderer.setStyle(videoElement, 'z-index', '1');
      this.renderer.appendChild(this.previewElement.nativeElement, videoElement);
    } finally {
      this.isInitializing = false;
    }
  }

  private initializeTracks(kind?: MediaDeviceKind, deviceId?: string) {
    if (kind) {
      switch (kind) {
        case 'audioinput':
          return createLocalTracks({ audio: { deviceId }, video: true });
        case 'videoinput':
          return createLocalTracks({ audio: true, video: { deviceId } });
      }
    }

    return createLocalTracks({ audio: true, video: true });
  }

  stringGen(len) {
    var text = "";

    var charset = "abcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < len; i++)
      text += charset.charAt(Math.floor(Math.random() * charset.length));

    return text;
  }

  loadRoom() {
    var url = `${environment.baseUrl}ChatVideo/Tokenization2/` + this.sessionId + "?id2=" + this.stringGen(8);
    let requestHeaders = new HttpHeaders();
    requestHeaders = requestHeaders.set('Access-Control-Allow-Origin', '*');
    requestHeaders = requestHeaders.set('Access-Control-Allow-Methods', 'GET, POST, DELETE, PUT');
    requestHeaders = requestHeaders.set('Access-Control-Allow-Headers', 'Origin, Content-Type, X-Auth-Token');
    let that = this;
    this.http.get(url, { headers: requestHeaders }).subscribe(data => {
      var obj = (data as TokenModel);
      connect(obj.token, { name: that.sessionId + "_room" }).then(room => {
        this.activeRoom = room;
        //console.log(`Successfully joined a Room: ${room}`);
        room.participants.forEach(participant1 => {
          
            participant1.on('trackSubscribed', function (track1) {
                if (track1.kind === 'video') {
                  const videoElement2 = track1.attach();
                  that.renderer.setStyle(videoElement2, 'position', 'fixed');
                  that.renderer.setStyle(videoElement2, 'min-width', '100%');
                  that.renderer.setStyle(videoElement2, 'min-height', '100%');
                  that.renderer.setStyle(videoElement2, 'right', '0');
                  that.renderer.setStyle(videoElement2, 'bottom', '0');
                  that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);
                }

              });
          });
        room.on('participantConnected', participant => {
          //setInterval(function () { alert("Hello"); }, 3000);
          setTimeout(function () { that.terminateRoomafterthirtyminutes() }, 1800000);
          room.participants.forEach(participant1 => {
           
              participant1.on('trackPublished', track => {
                //console.log(`Participant "${participant1.identity}" published ${track.kind} Track ${track.sid}`);
                if (track.kind === 'data') {
                  track.on('message', data => {
                  });
                }
              });
              participant1.on('trackSubscribed', function (track1) {
                 
                if (track1.kind === 'video') {
                  const videoElement2 = track1.attach();
                  that.renderer.setStyle(videoElement2, 'position', 'fixed');
                  that.renderer.setStyle(videoElement2, 'min-width', '100%');
                  that.renderer.setStyle(videoElement2, 'min-height', '100%');
                  that.renderer.setStyle(videoElement2, 'right', '0');
                  that.renderer.setStyle(videoElement2, 'bottom', '0');
                  that.renderer.appendChild(that.previewElement.nativeElement, videoElement2);
                 
                }

              });
              participant1.on('trackUnsubscribed', function (track1) {
                 
                ////console.log('gendy');
              });
             
            });

          });
        }, error => {
          console.error(`Unable to connect to Room: ${error.message}`);
        });
    });
  }

  async leaveRoom() {
    //console.log('Leaving room...');
    if (this.activeRoom) {
      this.activeRoom.disconnect();
      window.location.href = `${environment.baseUrl}/home`;
    }

  }

  async terminateRoomafterthirtyminutes() {
    //console.log("session ending ...");
     
    if (this.activeRoom) {
      this.activeRoom.disconnect();
      window.location.href = `${environment.baseUrl}/home`;
    }
  }

  async checkTheSession() {
    var url = `${environment.baseUrl}api/SessionApi/CheckSession/` + this.sessionId;

    let requestHeaders = new HttpHeaders();
    requestHeaders = requestHeaders.set('Access-Control-Allow-Origin', '*');
    requestHeaders = requestHeaders.set('Access-Control-Allow-Methods', 'GET, POST, DELETE, PUT');
    requestHeaders = requestHeaders.set('Access-Control-Allow-Headers', 'Origin, Content-Type, X-Auth-Token');
    let that = this;
    await this.http.get(url, { headers: requestHeaders }).toPromise()
      .catch(err => this.handleError(err));
  }

  handleError(error) {
    //console.log(error);
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    window.alert(errorMessage);
    this.err = errorMessage;
    return throwError(errorMessage);
  }
}
