import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { HttpClientModule} from '@angular/common/http';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from "ngx-spinner";
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { TimeagoModule } from 'ngx-timeago';

//-não esquecer de exportar os módulos que forem ser usados.

@NgModule({
  declarations: [],
  imports: [ 
    CommonModule,
    HttpClientModule, 
    NgxGalleryModule,
    FileUploadModule,
    NgxSpinnerModule.forRoot({ type: 'ball-clip-rotate-pulse' }),
    TooltipModule.forRoot(),
    TabsModule.forRoot(),
    BsDropdownModule.forRoot(),
    PaginationModule.forRoot(),
    ButtonsModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TimeagoModule.forRoot(),
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right',
      preventDuplicates: true,
      iconClasses:{
        error: 'toast-error',
        info: 'toast-info',
        success: 'toast-success',
        warning: 'toast-warning',
      },
      progressBar:true,
      timeOut: 2000
    })
  ],
  exports:[
    BsDropdownModule,
    ToastrModule,
    PaginationModule,
    TabsModule,
    NgxGalleryModule,
    NgxSpinnerModule,
    BsDatepickerModule,
    ButtonsModule,
    TimeagoModule,
    FileUploadModule
  ]
})
export class SharedModule { }
