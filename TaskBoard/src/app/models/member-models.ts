export enum UserLevelAccess {
    Reader = "Reader",
    Editor = "Editor",
    Admin = "Admin",
}

export interface BoardMemberDto {
    userId: string,
    email: string,
    userLevel: UserLevelAccess
}