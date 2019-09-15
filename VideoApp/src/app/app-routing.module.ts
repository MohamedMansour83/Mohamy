import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { VideoMohamyComponent } from './videomohamy/videomohamy.component';

//const routes: Routes = [
//  { path: "gendy", redirectTo: '/gendy' } ];

const routes2: Routes = [
  {
    path: '',
    component: VideoMohamyComponent
    ,
    children: [
      { path: 'videomohamy/:id', component: VideoMohamyComponent },
    ]
  },
  //{ path: 'videomohamy/:id', component: VideoMohamyComponent },

  //{
  //  path: 'app1',
  //  component: AppComponent
  //  //,
  //  //children: [
  //  //  { path: 'videomohamy/:id', component: VideoMohamyComponent }
  //  //]
  //},

  //{ path: 'videomohamy/:id', component: VideoMohamyComponent },
  //{ path: '', component: AppComponent },

  { path: "**", redirectTo: '/' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes2)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
