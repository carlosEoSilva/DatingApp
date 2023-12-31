import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/paginations';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl= environment.apiUrl;
  members:Member[]= [];
  user:User|undefined;
  userParams: UserParams|undefined;

  //-5  
  memberCache= new Map();

  constructor(private http:HttpClient, private accountService:AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user=> {
        if(user){
          this.userParams= new UserParams(user);
          this.user= user;
        }
      }
    })
  }

  getUserParams(){
    return this.userParams;
  }

  setUserParams(params:UserParams){
    this.userParams= params;
  }

  resetUserParams(){
    if(this.user){
      this.userParams= new UserParams(this.user);
      return this.userParams;
    }
  }
  //-todos os métodos retornam um observable
  getMembers(userParams:UserParams){

    //-4
    // console.log(Object.values(userParams).join('-'));

    const response= this.memberCache.get(Object.values(userParams).join('-'));
    // console.log(this.memberCache);

    //-6
    if(response) return of(response);

    let params= getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params= params.append('minAge', userParams.minAge);
    params= params.append('maxAge', userParams.maxAge);
    params= params.append('gender', userParams.gender);
    params= params.append('orderBy', userParams.orderBy);

    return getPaginationResult<Member[]>(this.baseUrl + 'users', params, this.http).pipe(
      map(response =>{
        //-7
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    )
    
  }

  getMember(username:string){
    //-8 
    const member= [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member:Member) => member.userName === username);
    
    if(member) return of(member);
    
      return this.http.get<Member>(this.baseUrl + "users/" + username); 
  }

  updateMember(member:Member){
    return this.http.put(this.baseUrl + 'users', member)
    .pipe(
      map(()=>{
        const index= this.members.indexOf(member);
        this.members[index]= {...this.members[index], ...member}
      })
    );
  }

  setMainPhoto(photoId:number){
    //-o objeto vazio no final é porque é uma requisição 'put'.
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId:number){
    return this.http.delete(this.baseUrl + "users/delete-photo/" + photoId);
  }

  addLike(username: string){
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  getLikes(predicate: string, pageNumber:number, pageSize:number){
    let params= getPaginationHeaders(pageNumber, pageSize);

    params= params.append('predicate', predicate);

    return getPaginationResult<Member[]>(this.baseUrl + 'likes', params, this.http);
  }

}
