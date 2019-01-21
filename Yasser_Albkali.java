package yasser_albkali;

import java.util.Scanner;
public class Yasser_Albkali 
{
    static void printDivisors(int n) 
    { 
        
        for (int i=1;i<=n;i++) 
            if (n%i==0) 
                System.out.printf("%d ",i); 
    } 
    public static void main(String args[]) 
    { 
        Scanner sc = new Scanner(System.in); 
        int x = sc.nextInt();
        System.out.println("The divisors of  " + x); 
        printDivisors(x);
    } 
}