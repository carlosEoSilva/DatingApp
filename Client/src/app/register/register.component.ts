import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, ValidatorFn, FormControl, FormGroup, Validators } from '@angular/forms';
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
  
    ngOnInit(): void {
      this.initializeForm();
    }

  //-o input é usado para receber valores de componentes filhos.
  //-@Input() usersFromHomeComponent:any;

  //-o output é para enviar valores para componetes pais.
  @Output() cancelRegister= new EventEmitter();
  model:any={};
  registerForm:FormGroup= new FormGroup({});

  register(){
    console.log(this.registerForm?.value);

    // this.accountService.register(this.model)
    // .subscribe({
    //   next: response => {
    //     console.log(response);
    //     this.cancel();
    //   },
    //   error: error => {
    //     console.error(error);
    //     this.toastr.error(error.error);
    //   }
    // });

    //-o .subscribe pode estar vazio, mas sem ele não acontece nada.
    // this.accountService.register(this.model)
    // .subscribe({});
  }

  cancel(){
    this.cancelRegister.emit(false);
    console.log('cancelled');
  }

  initializeForm(){
    this.registerForm= new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')])
    });

    this.registerForm.controls['password'].valueChanges.subscribe({
      next: ()=>{
        this.registerForm.controls['confirmPassword'].updateValueAndValidity();
      }
    })

  }

  matchValues(matchTo:string):ValidatorFn {
    return(control:AbstractControl)=>{
      return control.value === control.parent?.get(matchTo)?.value 
      ? null
      : {notMatching: true}
    }
  }

}
