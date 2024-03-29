import { CodeModel } from '@ngstack/code-editor/public_api';
import { CheckPoint } from '../service/userproject.service';

export enum FileNodeType {
    csharp = 'csharp',
    java = 'java',
    python = 'python',
    cpp = 'cpp',
    c = 'c',
    folder = 'folder'
}

export class FileNode {
    projectid?: number;
    projectType?: number;
    fileId?: number;
    children?: FileNode[];
    name: string;
    type: FileNodeType;
    exerciseId?: number;
    modified?:boolean;
    stdin?:string;
    checkpoints?:CheckPoint[]
    code?: CodeModel;

    constructor(name: string, type: FileNodeType, code: string,) {
        this.name = name;
        this.type = type;
        this.code = { language: type, value: code, uri: name };
    }
}
