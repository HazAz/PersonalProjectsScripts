using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the node structure. Each node has a left child, right child, middle child and a string task
public class Node{
	private Node LeftChild;
	private Node middleChild;
	private Node rightChild;
	private string task;

	//a bunch of constructors
	public Node(string s){
		this.task = s;
		this.LeftChild = null;
		this.middleChild = null;
		this.rightChild = null;
	}

	public Node(string s, Node l, Node m, Node r){
		this.task = s;
		this.LeftChild = l;
		this.middleChild = m;
		this.rightChild = r;
	}

	public Node(string s, Node l){
		this.task = s;
		this.LeftChild = l;
		this.middleChild = null;
		this.rightChild = null;
	}

	public Node(string s, Node l, Node m){
		this.task = s;
		this.LeftChild = l;
		this.middleChild = m;
		this.rightChild = null;
	}

	//Set and Get methods for the attributes
	public void SetLeft(Node n){
		this.LeftChild = n;
	}

	public Node GetLeft() {
		return this.LeftChild;
	}

	public void SetMid(Node n){
		this.middleChild = n;
	}

	public Node GetMid() {
		return this.middleChild;
	}

	public void SetRight(Node n){
		this.rightChild = n;
	}

	public Node GetRight() {
		return this.rightChild;
	}

	public string GetTask(){
		return task;
	}
}
