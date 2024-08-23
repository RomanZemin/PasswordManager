export default function Button({children, ...props}){
    return(
        <button {...props}>
            {children}
        </button>
    )
}