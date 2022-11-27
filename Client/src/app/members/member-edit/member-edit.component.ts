import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  //-o '@ViewChild' é para poder acessar a variável template 'editForm' no html.
  @ViewChild('editForm') editForm:NgForm|undefined;

  //-o '@HostListener' é para termos acesso aos eventos do host que neste caso é o navegador, esta parte do script impede trocar de url sem
  //-confirmação do usuário.
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
    if(this.editForm?.dirty){
      $event.returnValue= true;
    }
  }
  member: Member|undefined;
  user: User|null=null;

  constructor(
    private toastr:ToastrService, 
    private accountService:AccountService, 
    private memberService:MembersService) { 
    //-o usuário logado fica armazenado do 'currentUser$' do 'accountService'.
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user= user
    })
  }
  
  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    if(!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member=> this.member= member
    });
  }

  updateMember(){
    this.memberService.updateMember(this.editForm?.value)
    .subscribe({
      next: response=> {
        this.toastr.success('Profile updated!');
        //-o form será resetado usando o valor passado como parâmetro. Foi usado o '?' porque existe a possibilidade de o form estar vazio
        //-quando o componente é inicializado. 
        this.editForm?.reset(this.member);
      }
    });
    
  }


}
