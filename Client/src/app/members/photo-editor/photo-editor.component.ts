import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { environment } from 'src/environments/environment';
import { take } from 'rxjs/operators';
import { MembersService } from 'src/app/_services/members.service';
import { Photo } from 'src/app/_models/photo';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member:Member|undefined;
  uploader:FileUploader|undefined;
  user: User|undefined;
  mainPhoto:Photo;
  hasBaseDropZoneOver= false;
  baseUrl= environment.apiUrl;

  constructor(
    private accountService:AccountService, 
    private toastr:ToastrService,
    private memberService:MembersService) { 
    this.accountService.currentUser$
    .pipe(take(1))
    .subscribe({
      next: user=>{
        if(user){
          this.user= user;
        }
      }
    })
  }

  ngOnInit(): void { 
    this.initializeUploader();
  }

  fileOverBase(e:any){
    this.hasBaseDropZoneOver= e;
    // console.log("base over");
  }

  setMainPhoto(photo:Photo){
    //-tem que faze o 'subscribe' porque o 'setMainPhoto' é uma requisição 'http'.
    this.memberService.setMainPhoto(photo.id)
    .subscribe({
      next:()=>{
        if(this.user && this.member){
          this.user.photoUrl= photo.url;
          //-o usuário está sendo setado novamente para que os outros componentes também sejam atualizados.
          this.accountService.setCurrentUser(this.user);
          this.member.photoUrl= photo.url;
          this.member.photos.forEach(p=>{
            if(p.isMain) p.isMain= false;
            if(p.id === photo.id) p.isMain= true;
          })

        }
      }
    })
  }

  initializeUploader(){
    this.uploader= new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken:'Bearer ' + this.user?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10*1024*1024
    });

    this.uploader.onAfterAddingFile= (file)=>{
      file.withCredentials= false;
    }
    
    this.uploader.onSuccessItem= (item, response, status, headers)=>{
      if(response){
        const photo= JSON.parse(response);
        this.member?.photos.push(photo);

        if(photo.isMain && this.user && this.member){
          this.user.photoUrl= photo.url;
          this.member.photoUrl= photo.url;
          this.accountService.setCurrentUser(this.user);
        }
      }
    }
  
  
  }

  deletePhoto(photoId:number, isMain:boolean){
    if(isMain){
       this.toastr.warning("The main photo can't be deleted!");
    }
    else{
      this.memberService.deletePhoto(photoId).subscribe({
        next:()=>{
          if(this.member){
            //-o '.filter' está retornando todos elementos exceto o que tiver o 'id' correspondente.
            this.member.photos= this.member.photos.filter(x => x.id !== photoId);
          }
        }
      })
    }
  }

}