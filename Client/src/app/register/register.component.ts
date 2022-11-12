import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  constructor(
    private accountService:AccountService, 
    private toastr:ToastrService) { }
  
    ngOnInit(): void { }

  //-o input é usado para receber valores de componentes filhos.
  // @Input() usersFromHomeComponent:any;

  //-o output é para enviar valores para componetes pais.
  @Output() cancelRegister= new EventEmitter();
  model:any={};

  register(){
    this.accountService.register(this.model)
    .subscribe({
      next: response => {
        console.log(response);
        this.cancel();
      },
      error: error => {
        console.error(error);
        this.toastr.error(error.error);
      }
    });

    //-o .subscribe pode estar vazio, mas sem ele não acontece nada.
    // this.accountService.register(this.model)
    // .subscribe({});
  }

  cancel(){
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }

}
