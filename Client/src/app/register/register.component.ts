import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, ValidatorFn, FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({selector: 'app-register',templateUrl: './register.component.html',styleUrls: ['./register.component.css']})

export class RegisterComponent implements OnInit {
  //-o input é usado para receber valores de componentes filhos.
  //-@Input() usersFromHomeComponent:any;

  //-o output é para enviar valores para componetes pais.
  @Output() cancelRegister= new EventEmitter();
  registerForm:FormGroup= new FormGroup({});
  maxDate:Date= new Date();
  validationErrors:string[] | undefined;

  constructor(
    private accountService:AccountService,
    private router:Router,
    private fb:FormBuilder, 
    private toastr:ToastrService) { }
  
    ngOnInit(): void {
      this.initializeForm();
      this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
    }

  register(){
    const dob= this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const values= {...this.registerForm.value, dateOfBirth: dob};


    this.accountService.register(values)
    .subscribe({
      next: () => {
        this.router.navigateByUrl("/members");
      },
      error: error => {
        this.validationErrors= error;
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

  initializeForm(){
    this.registerForm= this.fb.group({
      gender:['male'],
      username: ['', Validators.required],
      knownas:['', Validators.required],
      dateOfBirth:['', Validators.required],
      city: ['', Validators.required],
      country:['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
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

  //-dob = date of birth 
  private getDateOnly(dob: string | undefined){
    if(!dob) return;
    let theDob= new Date(dob);
    return new Date(
      theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset()))
      .toISOString().slice(0, 10);
  }

}
