type Props = React.InputHTMLAttributes<HTMLInputElement> & { label: string }
export default function FormField({ label, ...props }: Props) {
  return (
    <label className="block mb-3">
      <span className="block text-sm font-medium mb-1">{label}</span>
      <input className="input" {...props} />
    </label>
  )
}
