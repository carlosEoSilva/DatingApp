import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({ providedIn: 'root'})

//-para usar o 'spinner', preciso de um 'interceptor'
export class BusyService {
  busyRequestCount= 0;

  constructor(private spinnerService:NgxSpinnerService) { }

  busy(){
    this.busyRequestCount++;
    this.spinnerService.show(undefined,{
      type:'ball-clip-rotate-pulse',
      bdColor:'rgba(250,250,250,0.7)',
      color:'#333333',
      fullScreen: true
    })
  }

  idle(){
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0){
      this.busyRequestCount= 0;
      this.spinnerService.hide();
    }
  }

}
