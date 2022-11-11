import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  constructor(private http:HttpClient) { }
  ngOnInit(): void { }

  registerMode= false;

  toggleRegisterMode(){
    this.registerMode= !this.registerMode;
  }


  //-função para receber o valor enviado pelo componente filho.
  cancelRegisterMode(event:boolean){
    this.registerMode= event;
  }


}
